import { Component, OnInit } from '@angular/core';

import { Hero }        from './hero';
import { HeroService } from './hero.service';

@Component({
  moduleId: module.id,
  selector: 'my-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: [ './dashboard.component.css' ]
})
export class DashboardComponent implements OnInit {
  heroes: Hero[] = [];
  items: any[] = [];
  
  constructor(private heroService: HeroService) { 
    this.items = [
{x: 51.6, y: 162.27272727272725, w: 64.8, h: 24.545454545454547},
{x: 159.6, y: 186.8181818181818, w: 64.8, h: 73.63636363636364},
{x: 267.6, y: 64.09090909090908, w: 64.8, h: 122.72727272727273},
{x: 375.6, y: 15, w: 64.8, h: 171.8181818181818},
{x: 483.6, y: 186.8181818181818, w: 64.8, h: 98.18181818181819}
    ];
  }

  ngOnInit(): void {
    this.heroService.getHeroes()
      .then(heroes => this.heroes = heroes.slice(1, 5));

    this.items = [
{x: 51.6, y: 162.27272727272725, w: 64.8, h: 24.545454545454547},
{x: 159.6, y: 186.8181818181818, w: 64.8, h: 73.63636363636364},
{x: 267.6, y: 64.09090909090908, w: 64.8, h: 122.72727272727273},
{x: 375.6, y: 15, w: 64.8, h: 171.8181818181818},
{x: 483.6, y: 186.8181818181818, w: 64.8, h: 98.18181818181819}
    ];
  }
}
