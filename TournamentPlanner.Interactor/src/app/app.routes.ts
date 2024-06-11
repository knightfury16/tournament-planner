import { Routes } from '@angular/router';
import { NumberDisplayTestComponent } from './number-display-test/number-display-test.component';
import { CashRegisterComponent } from './cash-register/cash-register.component';

export const routes: Routes = [ 
    {path:'digit', component:NumberDisplayTestComponent, pathMatch:'full'},
    {path:'', component:CashRegisterComponent}

];
