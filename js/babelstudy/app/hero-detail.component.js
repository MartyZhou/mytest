import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { Location } from '@angular/common';
import 'rxjs/add/operator/switchMap';

import { HeroService } from './hero.service';

@Component({
    selector: 'my-hero-detail',
    //inputs: ['hero'],
    templateUrl: './app/hero-detail.component.html',
    styleUrls: ['./app/hero-detail.component.css']
})

export class HeroDetailComponent implements OnInit {
    constructor(heroService, activatedRoute, location) {
        this.heroService = heroService;
        this.activatedRoute = activatedRoute;
        this.location = location;
    }

    static get parameters() {
        return [[HeroService], [ActivatedRoute], [Location]]
    }

    ngOnInit() {
        this.activatedRoute.params
            .switchMap((params) => this.heroService.getHero(+params['id']))
            .subscribe(data => this.hero = data);
    }

    goBack() {
        this.location.back();
    }

}
