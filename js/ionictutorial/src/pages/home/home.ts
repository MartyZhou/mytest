import { Component, OnInit } from '@angular/core';

import { NavController } from 'ionic-angular';

import { Hero } from '../../app/hero';
import { HeroService } from '../../app/hero.service';
import { HeroDetailComponent } from '../../pages/home/hero-detail.component';

@Component({
  selector: 'page-home',
  templateUrl: 'home.html'
})
export class HomePage implements OnInit {
  heroes: Hero[] = [];

  constructor(public navCtrl: NavController, private heroService: HeroService) {

  }

  ngOnInit(): void {
    this.heroService.getHeroes()
      .then(heroes => this.heroes = heroes.slice(1, 5));
  }

  onSelect(hero: Hero): void {
    this.navCtrl.push(HeroDetailComponent, hero);
  }
}
