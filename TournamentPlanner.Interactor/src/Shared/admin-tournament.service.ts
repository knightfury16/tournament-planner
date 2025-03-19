import { inject, Inject, Injectable } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { TP_BASE_URL } from '../app/app.config';
import { HttpClient, HttpParams } from '@angular/common/http';
import { AddMatchScoreDto, CanIDrawDto, CanIScheduleDto, DrawDto, MatchDto, ScheduleingInfo, TournamentDto, TournamentStatusChangeDto } from '../app/tp-model/TpModel';

@Injectable({
  providedIn: 'root'
})
export class AdminTournamentService {

  private baseUrl: string = inject(TP_BASE_URL);
  private httpClient: HttpClient = inject(HttpClient);

  public getAdminTournaments(
    name: string = '',
    searchCategory?: string,
    status?: string,
    gameType?: string,
    startDate?: string,
    endDate?: string
  ): Promise<TournamentDto[]> {

    let params = new HttpParams();

    if (name) params = params.set('name', name);
    if (searchCategory) params = params.set('searchCategory', searchCategory);
    if (status) params = params.set('status', status);
    if (gameType) params = params.set('gameType', gameType);
    if (startDate) params = params.set('startDate', startDate);
    if (endDate) params = params.set('endDate', endDate);
    return firstValueFrom(this.httpClient.get<TournamentDto[]>(`${this.baseUrl}/tournament/created/admin`, { withCredentials: true, params }));
  }
  public changeTournamentStatus(tournamentId: string, changedStatus: string): Promise<{ message: string }> {
    //i have to do this coz server is expecting a json object, and if string is sent it can not deserialize
    var tournamentStatusChangeDto: TournamentStatusChangeDto = { tournamentStatus: changedStatus };
    return firstValueFrom(this.httpClient.post<{ message: string }>(`${this.baseUrl}/tournament/${tournamentId}/change-status`, tournamentStatusChangeDto, { withCredentials: true, headers: { 'Content-Type': 'application/json' } }))
  }

  public addMatchScore(matchId: string, score: AddMatchScoreDto): Promise<MatchDto> {
    return firstValueFrom(this.httpClient.post<MatchDto>(`${this.baseUrl}/matches/${matchId}/entry-match-score`, score, { withCredentials: true, headers: { 'Content-Type': 'application/json' } }));

  }

  public makeMatchSchedule(tournamentId: string, schedulingInfo: ScheduleingInfo): Promise<MatchDto[]> {
    return firstValueFrom(this.httpClient.post<MatchDto[]>(`${this.baseUrl}/tournament/${tournamentId}/make-schedule`, schedulingInfo, { withCredentials: true, headers: { 'Content-Type': 'application/json' } }));

  }

  public canIDraw(tournamentId: string): Promise<CanIDrawDto> {
    return firstValueFrom(this.httpClient.get<CanIDrawDto>(`${this.baseUrl}/tournament/${tournamentId}/can-make-draw`, { withCredentials: true }));
  }
  public canISchedule(tournamentId: string): Promise<CanIScheduleDto> {
    return firstValueFrom(this.httpClient.get<CanIScheduleDto>(`${this.baseUrl}/tournament/${tournamentId}/can-make-schedule`, { withCredentials: true }));
  }

  public makeDraws(tournamentId: string, seededPlayersId: string[] = [], matchTypePrefix: string = "" ): Promise<DrawDto[]> {
    var payload =
    {
      SeedersId: seededPlayersId.length ? seededPlayersId : undefined, // Send undefinef if empty
      matchTypePrefix: matchTypePrefix || undefined  // Avoid Sending empty string
    }
    return firstValueFrom(this.httpClient.post<DrawDto[]>(`${this.baseUrl}/tournament/${tournamentId}/make-draw`, payload, { withCredentials: true, headers: { 'Content-Type': 'application/json' } }));
  }
}
