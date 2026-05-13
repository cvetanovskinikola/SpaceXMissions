import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { LaunchDto } from '../models/launch.model';
import { ApiService } from './api.service';

@Injectable({ providedIn: 'root' })
export class SpaceXService {
  private readonly api = inject(ApiService);

  getLatest(): Observable<LaunchDto> {
    return this.api.get<LaunchDto>('/launches/latest');
  }

  getUpcoming(): Observable<LaunchDto[]> {
    return this.api.get<LaunchDto[]>('/launches/upcoming');
  }

  getPast(): Observable<LaunchDto[]> {
    return this.api.get<LaunchDto[]>('/launches/past');
  }
}