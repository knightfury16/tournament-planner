import { inject } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivateFn, RouterStateSnapshot } from "@angular/router";
import { AuthService } from "../Shared/auth.service";
import { DomainRole } from "../app/tp-model/TpModel";

export const adminGuard: CanActivateFn = async (route: ActivatedRouteSnapshot, state: RouterStateSnapshot) => {
    const authService = inject(AuthService);

    await authService.initializeUserInfo();
    var currentUser = authService.getCurrentUser();

    if (currentUser?.role === DomainRole.Admin) {
        return true;
    }

    console.log("Only Admin can access this route");
    return false;
}