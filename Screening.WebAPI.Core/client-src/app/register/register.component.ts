import {Component, OnDestroy, OnInit} from "@angular/core";
import {ActivatedRoute, Router} from "@angular/router";
import {Subscription} from "rxjs/Subscription";
import {AuthenticationService} from "../services/authentication.service";
import {AlertService} from "../alert.service";

@Component({
    selector: "app-register",
    templateUrl: "./register.component.html",
    styleUrls: ["./register.component.less"]
})
export class RegisterComponent implements OnInit, OnDestroy {
    username = "";
    password = "";
    confirmPassword = "";
    role = "student";
    private sub: Subscription;
    loading = false;

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private alertService: AlertService,
        private authenticationService: AuthenticationService) {
    }

    ngOnInit() {
        this.sub = this.route
            .queryParams
            .subscribe(params => {
                // Defaults to 0 if no query param provided.
                this.role = params["role"] || "student";
            });
    }

    ngOnDestroy() {
        this.sub.unsubscribe();
    }

    register() {
        this.loading = true;
        this.authenticationService.register(this.username, this.password, this.confirmPassword, this.role).subscribe(
            data => {
                this.router.navigate(["login"]);
            },
            error => {
                this.alertService.error(error);
                this.loading = false;
            });
    }
}
