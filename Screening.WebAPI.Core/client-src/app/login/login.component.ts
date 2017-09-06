import {Component, OnInit} from "@angular/core";
import {ActivatedRoute, Router} from "@angular/router";
import {AuthenticationService} from "../services/authentication.service";
import {AlertService} from "../alert.service";

@Component({
    selector: "app-login",
    templateUrl: "./login.component.html",
    styleUrls: ["./login.component.less"]
})
export class LoginComponent implements OnInit {
    username = "";
    password = "";
    isRememberMe = "";
    loading = false;

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private authenticationService: AuthenticationService,
        private alertService: AlertService) {
    }

    ngOnInit() {
        // reset login status
        this.authenticationService.logout();
    }

    login() {
        this.loading = true;
        this.authenticationService.login(this.username, this.password)
            .subscribe(
                user => {
                    this.router.navigate([`/${user.roleName}`]);
                },
                error => {
                    this.alertService.error(error);
                    this.loading = false;
                });
    }
}
