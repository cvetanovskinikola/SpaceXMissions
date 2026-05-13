import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit, OnDestroy, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTabChangeEvent, MatTabsModule } from '@angular/material/tabs';
import { MatToolbarModule } from '@angular/material/toolbar';
import { Router } from '@angular/router';
import { Observable, Subject, of } from 'rxjs';
import { catchError, finalize, switchMap, takeUntil } from 'rxjs/operators';
import { LaunchDto, MissionType } from '../../core/models/launch.model';
import { AuthService } from '../../core/services/auth.service';
import { SpaceXService } from '../../core/services/spacex.service';
import { LaunchDetailComponent } from './launch-detail/launch-detail.component';

@Component({
  selector: 'app-spacex',
  standalone: true,
  imports: [
    CommonModule,
    MatButtonModule,
    MatIconModule,
    MatListModule,
    MatProgressSpinnerModule,
    MatTabsModule,
    MatToolbarModule,
    LaunchDetailComponent
  ],
  templateUrl: './spacex.component.html',
  styleUrls: ['./spacex.component.scss']
})
export class SpaceXComponent implements OnInit, OnDestroy {
  private readonly spacexService = inject(SpaceXService);
  private readonly router = inject(Router);
  readonly authService = inject(AuthService);

  public readonly MissionType = MissionType;

  public selectedType: MissionType = MissionType.Latest;
  public launches: LaunchDto[] = [];
  public selectedLaunch: LaunchDto | null = null;

  public loading = false;
  public error: string | null = null;

  private readonly destroy$ = new Subject<void>();
  private readonly missionType$ = new Subject<MissionType>();

  public ngOnInit(): void {
    this.missionType$
      .pipe(
        switchMap(type => this.getMissions(type)),
        takeUntil(this.destroy$)
      )
      .subscribe(launches => {
        this.launches = launches;
        this.selectedLaunch = launches[0] ?? null;
      });

    this.loadMissions(this.selectedType);
  }

  public ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  public onTabChange(event: MatTabChangeEvent): void {
    const types = [MissionType.Latest, MissionType.Upcoming, MissionType.Past];
    this.selectedType = types[event.index];
    this.loadMissions(this.selectedType);
  }

  public onLaunchSelected(launch: LaunchDto): void {
    this.selectedLaunch = launch;
  }

  public signOut(): void {
    this.authService.signOut();
    this.router.navigate(['/signin']);
  }

  private loadMissions(type: MissionType): void {
    this.loading = true;
    this.error = null;
    this.launches = [];
    this.selectedLaunch = null;
    this.missionType$.next(type);
  }

  private getMissions(type: MissionType): Observable<LaunchDto[]> {
    const source$ = type === MissionType.Latest
      ? this.spacexService.getLatest().pipe(
          switchMap(launch => of([launch]))
        )
      : type === MissionType.Upcoming
        ? this.spacexService.getUpcoming()
        : this.spacexService.getPast();

    return source$.pipe(
      catchError((err: HttpErrorResponse) => {
        this.error = this.extractErrorMessage(err);
        return of([] as LaunchDto[]);
      }),
      finalize(() => (this.loading = false))
    );
  }

  private extractErrorMessage(err: HttpErrorResponse): string {
    if (err.status === 502) {
      return 'The SpaceX service is currently unavailable. Please try again later.';
    }
    if (err.status === 0) {
      return 'Cannot reach the server. Check your connection.';
    }
    return 'Something went wrong while loading missions.';
  }
}
