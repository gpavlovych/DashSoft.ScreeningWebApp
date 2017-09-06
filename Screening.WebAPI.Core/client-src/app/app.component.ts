import {Component, OnInit} from "@angular/core";
import {NavbarService} from "./services/navbar.service";
import {AuthenticationService} from "./services/authentication.service";

@Component({
    selector: "app-root",
    templateUrl: "./app.component.html",
    styleUrls: ["./app.component.less"]
})
export class AppComponent implements OnInit {
    constructor(
        public navbarService: NavbarService,
        public authenticationService: AuthenticationService) {
    }

    ngOnInit() {

    }
}
