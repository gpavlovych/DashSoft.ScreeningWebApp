import { Injectable } from "@angular/core";
import {RoleBasedGuard} from "./role-based.guard";

@Injectable()
export class TeacherGuard extends RoleBasedGuard {
    protected getRoleName(): string {
        return "teacher";
    }
}
