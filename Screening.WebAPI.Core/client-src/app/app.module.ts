import { BrowserModule } from "@angular/platform-browser";
import { NgModule } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { BsDropdownModule} from "ngx-bootstrap";

import { AppRoutingModule } from "./app-routing.module";
import { AppComponent } from "./app.component";
import { StudentComponent } from "./student/student.component";
import { LandingComponent } from "./landing/landing.component";
import { LoginComponent } from "./login/login.component";
import { RegisterComponent } from "./register/register.component";
import { TeacherComponent } from "./teacher/teacher.component";
import { ParentComponent } from "./parent/parent.component";
import { PathNotFoundComponent } from "./path-not-found/path-not-found.component";
import { AlertComponent } from "./alert/alert.component";
import {AuthenticationService} from "./services/authentication.service";
import {AlertService} from "./alert.service";
import {HttpModule} from "@angular/http";
import {StudentGuard} from "./guards/student.guard";
import {ParentGuard} from "./guards/parent.guard";
import {TeacherGuard} from "./guards/teacher.guard";
import {NavbarService} from "./services/navbar.service";
import {PublicGuard} from "./guards/public.guard";
import {PublicNoNavbarGuard} from "./guards/public-no-navbar.guard";
import {UserService} from "./services/user.service";
import {MarkService} from "./services/mark.service";

@NgModule({
    declarations: [
        AppComponent,
        StudentComponent,
        LandingComponent,
        LoginComponent,
        RegisterComponent,
        TeacherComponent,
        ParentComponent,
        PathNotFoundComponent,
        AlertComponent
    ],
    imports: [
        BrowserModule,
        AppRoutingModule,
        FormsModule,
        BsDropdownModule.forRoot(),
        HttpModule
    ],
    providers: [
        NavbarService,
        AuthenticationService,
        AlertService,
        UserService,
        MarkService,
        StudentGuard,
        TeacherGuard,
        ParentGuard,
        PublicGuard,
        PublicNoNavbarGuard
    ],
    bootstrap: [AppComponent]
})
export class AppModule {
}
