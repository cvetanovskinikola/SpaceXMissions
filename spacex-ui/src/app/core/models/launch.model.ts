export interface LaunchDto {
  id: string;
  name: string;
  dateUtc: string;
  flightNumber: number;
  success: boolean | null;
  upcoming: boolean;
  details: string | null;
  patchImageUrl: string | null;
  webcastUrl: string | null;
  articleUrl: string | null;
  wikipediaUrl: string | null;
}

export enum MissionType {
  Latest = 'latest',
  Upcoming = 'upcoming',
  Past = 'past'
}
