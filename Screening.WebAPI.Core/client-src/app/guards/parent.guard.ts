import { Injectable } from "@angular/core";
import {RoleBasedGuard} from "./role-based.guard";

@Injectable()
export class ParentGuard extends RoleBasedGuard {
    protected getRoleName(): string {
        return "parent";
    }
}
