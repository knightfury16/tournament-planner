import { Routes } from '@angular/router';
import { NumberDisplayTestComponent } from './number-display-test/number-display-test.component';
import { CashRegisterComponent } from './cash-register/cash-register.component';
import { PeopleListComponent } from './people-list/people-list.component';
import { TripListComponent } from './trip-list/trip-list.component';
import { AirportsListComponent } from './airports-list/airports-list.component';
import { TournamentListComponent } from './tournament-list/tournament-list.component';
import { AddTournamentComponent } from './add-tournament/add-tournament.component';
import { LoginComponent } from './login/login.component';
import { RegisterPlayerComponent } from './register-player/register-player.component';
import { RegisterAdminComponent } from './register-admin/register-admin.component';
import { authGuard } from '../guards/authGuard';
import { adminGuard } from '../guards/adminGuard';
import { TestComponent } from './test/test.component';
import { TournamentDetailsHomepageComponent } from './tournament-details-homepage/tournament-details-homepage.component';
import { AdminCreatedTournamentListComponent } from './admin-created-tournament-list/admin-created-tournament-list.component';
import { ManageTournamentHomepageComponent } from './manage-tournament-homepage/manage-tournament-homepage.component';

export const routes: Routes = [
    { path: '', redirectTo: 'people', pathMatch: 'full' },
    { path: 'digit', component: NumberDisplayTestComponent, pathMatch: 'full' },
    { path: 'cash-register', component: CashRegisterComponent },
    { path: 'people', component: PeopleListComponent },
    { path: 'airports', component: AirportsListComponent },
    { path: 'people/:userName/trips', component: TripListComponent },
    { path: 'tp', component: TournamentListComponent },
    { path: 'tp/tournament-details-homepage/:tournamentId', component: TournamentDetailsHomepageComponent },
    { path: 'tp/manage-tournament-homepage/:tournamentId', component: ManageTournamentHomepageComponent, canActivate: [authGuard, adminGuard] },
    { path: 'tp/add-tournament', component: AddTournamentComponent, canActivate: [authGuard, adminGuard] },
    { path: 'tp/admin-created-tournament-list', component: AdminCreatedTournamentListComponent, canActivate: [authGuard, adminGuard] },
    { path: 'login', component: LoginComponent },
    { path: 'register-player', component: RegisterPlayerComponent },
    { path: 'register-admin', component: RegisterAdminComponent },
    { path: 'test', component: TestComponent },
];
