import { HttpClient, HttpParams } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { TP_BASE_URL } from './app.config';
import { Observable, of, retry } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class TournamentPlannerService {
  constructor(
    private httpClient: HttpClient,
    @Inject(TP_BASE_URL) private baseUrl: string
  ) {}

  public getTournament(name: string): Observable<any[]> {
    let params = new HttpParams();
    if (name) {
      params = params.set('name', name);
    }
    return this.httpClient.get<any[]>(`${this.baseUrl}/tournament`, { params });
  }

  public getMatches(tournamentId: number) {
    let params = new HttpParams();

    if (tournamentId) {
      params = params.set('tournamentId', tournamentId);
    }
    // /api/matches?tournamentId=21
    return this.httpClient.get<any[]>(`${this.baseUrl}/matches`, { params });
  }
}
