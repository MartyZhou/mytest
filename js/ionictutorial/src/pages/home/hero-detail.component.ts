import { Component, OnInit } from '@angular/core';

import { NavController } from 'ionic-angular';
import { NavParams } from 'ionic-angular';

import { Hero } from '../../app/hero';
import { HeroService } from '../../app/hero.service';

@Component({
    selector: 'my-hero-detail',
    templateUrl: './hero-detail.component.html'
})
export class HeroDetailComponent implements OnInit {
    hero: Hero;

    powers = ['Really Smart', 'Super Flexible',
        'Super Hot', 'Weather Changer'];

    constructor(private navCtrl: NavController, private heroService: HeroService, public navParams: NavParams) {
        this.hero = navParams.data;
    }

    ngOnInit(): void {
        console.log(this.hero.id);
    }

    onSubmit(): void {
        this.save();
    }

    save(): void {
        this.heroService.update(this.hero)
            .then(() => this.navCtrl.pop());
    }
}
