import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import {StudentComponent} from "./student/student.component";
import {TeacherComponent} from "./teacher/teacher.component";
import {ParentComponent} from "./parent/parent.component";
import {LandingComponent} from "./landing/landing.component";
import {LoginComponent} from "./login/login.component";
import {RegisterComponent} from "./register/register.component";
import {PathNotFoundComponent} from "./path-not-found/path-not-found.component";
import {TeacherGuard} from "./guards/teacher.guard";
import {StudentGuard} from "./guards/student.guard";
import {ParentGuard} from "./guards/parent.guard";
import {PublicGuard} from "./guards/public.guard";
import {PublicNoNavbarGuard} from "./guards/public-no-navbar.guard";

const routes: Routes = [
    {
        path: "student",
        component: StudentComponent,
        canActivate: [StudentGuard]
    },
    {
        path: "teacher",
        component: TeacherComponent,
        canActivate: [TeacherGuard]
    },
    {
        path: "parent",
        component: ParentComponent,
        canActivate: [ParentGuard]
    },
    {
        path: "landing",
        component: LandingComponent,
        canActivate: [PublicGuard]
    },
    {
        path: "login",
        component: LoginComponent,
        canActivate: [PublicNoNavbarGuard]
    },
    {
        path: "register",
        component: RegisterComponent,
        canActivate: [PublicGuard]
    },
    {
        path: "",
        component: LandingComponent,
        canActivate: [PublicGuard]
    },
    {
        path: "**",
        component: PathNotFoundComponent,
        canActivate: [PublicGuard]
    }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule {
}
