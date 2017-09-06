import { Injectable } from "@angular/core";
import {RoleBasedGuard} from "./role-based.guard";

@Injectable()
export class StudentGuard extends RoleBasedGuard {
    protected getRoleName(): string {
        return "student";
    }
}
