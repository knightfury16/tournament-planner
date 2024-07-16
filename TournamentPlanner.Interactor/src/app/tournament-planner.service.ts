import { HttpClient, HttpParams } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { TP_BASE_URL } from './app.config';
import { Observable, of, retry } from 'rxjs';
import { Match, Tournament, TournamentDto } from './tp-model/TpModel';

@Injectable({
  providedIn: 'root',
})
export class TournamentPlannerService {
  constructor(
    private httpClient: HttpClient,
    @Inject(TP_BASE_URL) private baseUrl: string
  ) {}

  public getTournament(name: string): Observable<Tournament[]> {
    let params = new HttpParams();
    if (name) {
      params = params.set('name', name);
    }
    return this.httpClient.get<Tournament[]>(`${this.baseUrl}/tournament`, { params });
  }

  public getMatches(tournamentId: number): Observable<Match[]> {
    let params = new HttpParams();

    if (tournamentId) {
      params = params.set('tournamentId', tournamentId);
    }
    return this.httpClient.get<Match[]>(`${this.baseUrl}/matches`, { params });
  }

  public addTournament(tournamentDto: TournamentDto): Observable<any[]> {
    return this.httpClient.post<any[]>(
      `${this.baseUrl}/tournament`,
      tournamentDto
    );
  }
}
