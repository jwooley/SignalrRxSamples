import { Component, OnInit } from '@angular/core';
import { Observable, combineLatest } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { SecurityService } from '../services/security';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-reactive-form',
  templateUrl: './reactiveform.component.html'
})

export class ReactiveFormComponent {
  constructor(
    public security: SecurityService,
    private http: HttpClient,
    private formBuilder: FormBuilder
  ) { }

  userForm: FormGroup = this.formBuilder.group({
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    password: [''],
    statusDescription: ['']
  })
  user$: Observable<User> = combineLatest(
    this.http.get<User>("api/Security/User/1"),
    this.http.get<Array<ILookup>>("api/Security/StatusCodes"))
    .pipe(
      map(([user, code]) => {
        user.statusDescription = code.find(c => c.code === user.statusId).description;
        return user;
      }), tap(user => this.userForm.patchValue(user))
    );
}

class User {
  firstName: string;
  lastName: string;
  password: string;
  userId: number;
  statusId: number;
  statusDescription: string;
}
interface ILookup {
  code: number;
  description: string;
}
