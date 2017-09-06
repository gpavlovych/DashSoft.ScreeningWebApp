import { Injectable } from "@angular/core";
import {CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router} from "@angular/router";
import { Observable } from "rxjs/Observable";
import {User} from "../models/user";
import {NavbarService} from "../services/navbar.service";

@Injectable()
export abstract class RoleBasedGuard implements CanActivate {
    protected abstract getRoleName(): string;

    constructor(private router: Router, private navbarService: NavbarService) {
    }

    canActivate(
        next: ActivatedRouteSnapshot,
        state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
        this.navbarService.show();
        const user: User = JSON.parse(localStorage.getItem("currentUser"));
        if (user.roleName === this.getRoleName()) {
            // logged in so return true
            return true;
        }

        // not logged in so redirect to login page with the return url
        this.router.navigate(["/page-not-found"]);
        return false;
    }
}
