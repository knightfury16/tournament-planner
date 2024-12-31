import { HttpClient, HttpParams } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { TP_BASE_URL } from './app.config';
import { firstValueFrom, Observable, of, retry } from 'rxjs';
import { AddTournamentDto, DrawDto, MatchDto, MatchTypeDto, PlayerDto, PlayerStandingDto, TournamentDto } from './tp-model/TpModel';

@Injectable({
  providedIn: 'root',
})
export class TournamentPlannerService {
  constructor(
    private httpClient: HttpClient,
    @Inject(TP_BASE_URL) private baseUrl: string
  ) { }

  public getTournament(
    name: string = '',
    searchCategory?: string,
    status?: string,
    gameType?: string,
    startDate?: string,
    endDate?: string
  ): Observable<TournamentDto[]> {
    let params = new HttpParams();

    if (name) params = params.set('name', name);
    if (searchCategory) params = params.set('searchCategory', searchCategory);
    if (status) params = params.set('status', status);
    if (gameType) params = params.set('gameType', gameType);
    if (startDate) params = params.set('startDate', startDate);
    if (endDate) params = params.set('endDate', endDate);

    return this.httpClient.get<TournamentDto[]>(`${this.baseUrl}/tournament`, { params });
  }
  public getTournamentById(id: string): Promise<TournamentDto> {
    return firstValueFrom(this.httpClient.get<TournamentDto>(`${this.baseUrl}/tournament/${id}`));
  }

  //getting the matches by the draw populated
  public getTournamentMatches(tournamentId: string): Promise<DrawDto[]> {
    return firstValueFrom(this.httpClient.get<DrawDto[]>(`${this.baseUrl}/tournament/${tournamentId}/matches`));
  }

  public getMatchById(matchId: string): Promise<MatchDto> {
    return firstValueFrom(this.httpClient.get<MatchDto>(`${this.baseUrl}/matches/${matchId}`))
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
  public getTournamentDraws(tournamentId: string): Promise<DrawDto[]> {
    return firstValueFrom(this.httpClient.get<DrawDto[]>(`${this.baseUrl}/tournament/${tournamentId}/get-draws`));

  }

  //match type means like GROUP-A, GROUP-B
  public getMatchType(matchTypeId: string): Promise<MatchTypeDto> {
    return firstValueFrom(this.httpClient.get<MatchTypeDto>(`${this.baseUrl}/match-type/${matchTypeId}`))
  }

  public getGroupStandingOfMatchType(matchTypeId: string): Promise<PlayerStandingDto[]> {
    return firstValueFrom(this.httpClient.get<PlayerStandingDto[]>(`${this.baseUrl}/match-type/${matchTypeId}/group-standing`));
  }

}
