import { Component, OnInit } from '@angular/core';
import { HeroService } from './hero.service';


@Component({
    //moduleId: module.id,
    selector: 'my-dashboard',
    templateUrl: './app/dashboard.component.html',
    styleUrls: ['./app/dashboard.component.css']
})

export class DashboardComponent implements OnInit {
    constructor(heroService) {
        this.heroService = heroService;
    }

    static get parameters() {
        return [[HeroService]];
    }

    ngOnInit() {
        this.getHeroes();
    }

    getHeroes() {
        //this.heroes = this.heroService.getHeroes().slice(1, 5);
        this.heroService.getHeroes().then(data => {
            this.heroes = data.slice(1, 5);
        });
    }
}
