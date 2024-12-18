import { Routes } from '@angular/router';
import { NumberDisplayTestComponent } from './number-display-test/number-display-test.component';
import { CashRegisterComponent } from './cash-register/cash-register.component';
import { PeopleListComponent } from './people-list/people-list.component';
import { TripListComponent } from './trip-list/trip-list.component';
import { AirportsListComponent } from './airports-list/airports-list.component';
import { TournamentListComponent } from './tournament-list/tournament-list.component';
import { TournamentMatchesComponent } from './tournament-matches/tournament-matches.component';
import { AddTournamentComponent } from './add-tournament/add-tournament.component';
import { LoginComponent } from './login/login.component';
import { RegisterPlayerComponent } from './register-player/register-player.component';

export const routes: Routes = [ 
    {path:'', redirectTo:'people', pathMatch:'full'},
    {path:'digit', component:NumberDisplayTestComponent, pathMatch:'full'},
    {path:'cash-register', component:CashRegisterComponent},
    {path:'people', component:PeopleListComponent},
    {path:'airports', component:AirportsListComponent},
    {path:'people/:userName/trips', component:TripListComponent},
    {path:'tp/:tournamentId/matches', component:TournamentMatchesComponent},
    {path:'tp', component:TournamentListComponent},
    {path:'tp/add-tournament', component:AddTournamentComponent},
    {path:'login', component:LoginComponent},
    {path:'register-player', component:RegisterPlayerComponent},
];
