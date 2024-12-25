import { HttpClient, HttpParams } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { TP_BASE_URL } from './app.config';
import { firstValueFrom, Observable, of, retry } from 'rxjs';
import { AddTournamentDto, MatchDto, PlayerDto, TournamentDto } from './tp-model/TpModel';

@Injectable({
  providedIn: 'root',
})
export class TournamentPlannerService {
  constructor(
    private httpClient: HttpClient,
    @Inject(TP_BASE_URL) private baseUrl: string
  ) { }

  public getTournament(name: string): Observable<TournamentDto[]> {
    let params = new HttpParams();
    if (name) {
      params = params.set('name', name);
    }
    return this.httpClient.get<TournamentDto[]>(`${this.baseUrl}/tournament`, { params });
  }
  public getTournamentById(id: string): Promise<TournamentDto> {
    return firstValueFrom(this.httpClient.get<TournamentDto>(`${this.baseUrl}/tournament/${id}`));
  }

  public getMatches(tournamentId: number): Observable<MatchDto[]> {
    let params = new HttpParams();

    if (tournamentId) {
      params = params.set('tournamentId', tournamentId);
    }
    return this.httpClient.get<MatchDto[]>(`${this.baseUrl}/matches`, { params });
  }

  public addTournament(addTournamentDto: AddTournamentDto): Observable<TournamentDto> {
    return this.httpClient.post<TournamentDto>(`${this.baseUrl}/tournament`, addTournamentDto, { withCredentials: true })
  }

  public getTournamentPlayers(tournamentId: string): Promise<PlayerDto[]> {
    return firstValueFrom(this.httpClient.get<PlayerDto[]>(`${this.baseUrl}/tournament/${tournamentId}/players`, { withCredentials: true }));
  }


  public registerPlayerInTournament(tournamentId: string): Promise<string> {
    return firstValueFrom(this.httpClient.post<string>(`${this.baseUrl}/tournament/register`, { tournamentId }, { withCredentials: true }));
  }
}
