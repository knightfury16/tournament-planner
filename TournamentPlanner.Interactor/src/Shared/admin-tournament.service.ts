import { inject, Inject, Injectable } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { TP_BASE_URL } from '../app/app.config';
import { HttpClient, HttpParams } from '@angular/common/http';
import { AddMatchScoreDto, MatchDto, TournamentDto, TournamentStatusChangeDto } from '../app/tp-model/TpModel';

@Injectable({
  providedIn: 'root'
})
export class AdminTournamentService {

  private baseUrl: string = inject(TP_BASE_URL);
  private httpClient: HttpClient = inject(HttpClient);

  public getAdminTournaments(): Promise<TournamentDto[]> {
    return firstValueFrom(this.httpClient.get<TournamentDto[]>(`${this.baseUrl}/tournament/created/admin`, { withCredentials: true }));
  }
  public changeTournamentStatus(tournamentId: string, changedStatus: string): Promise<{ message: string }> {
    //i have to do this coz server is expecting a json object, and if string is sent it can not deserialize
    var tournamentStatusChangeDto: TournamentStatusChangeDto = { tournamentStatus: changedStatus };
    return firstValueFrom(this.httpClient.post<{ message: string }>(`${this.baseUrl}/tournament/${tournamentId}/change-status`, tournamentStatusChangeDto, { withCredentials: true, headers: { 'Content-Type': 'application/json' } }))
  }

  public addMatchScore(matchId: string, score: AddMatchScoreDto): Promise<MatchDto> {
    return firstValueFrom(this.httpClient.post<MatchDto>(`${this.baseUrl}/matches/${matchId}/entry-match-score`, score, { withCredentials: true, headers: { 'Content-Type': 'application/json' } }));

  }
}
