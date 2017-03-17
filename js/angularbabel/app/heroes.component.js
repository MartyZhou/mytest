import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { HeroService } from './hero.service';

@Component({
    selector: 'my-heroes',
    templateUrl: './app/heroes.component.html',
    styleUrls: ['./app/heroes.component.css']
})

export class HeroesComponent implements OnInit {
    constructor(heroService, router) {
        this.heroService = heroService;
        this.router = router;
        this.heroes = [
            //{ id: 20, name: 'Tornado' }
        ];
    }

    static get parameters() {
        return [[HeroService], [Router]];
    }

    ngOnInit() {
        this.getHeroes();
    }

    onSelect(hero) {
        this.selectedHero = hero;
    }

    getHeroes() {
        //this.heroes = this.heroService.getHeroes();
        this.heroService.getHeroes().then(data => {
            this.heroes = data;
        });
    }

    gotoDetail() {
        this.router.navigate(['/detail', this.selectedHero.id]);
    }

}