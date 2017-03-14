import { Component } from '@angular/core';

@Component({
    selector: 'my-app',
    styleUrls: ['./app/app.component.css'],
    template: `
  <h1>{{title}}</h1>
  <input [ngModel]="title">
  <nav>
    <a routerLink="/dashboard" routerLinkActive="active">Dashboard</a>
    <a routerLink="/heroes" routerLinkActive="active">Heroes</a>
  </nav>
  <router-outlet></router-outlet>
`,

})

export class AppComponent {
    constructor() {
        this.title = 'Tour of Heroes';
    }
}
