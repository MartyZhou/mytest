import { Component } from '@angular/core';

import { Hero } from './hero';

@Component({
    moduleId: module.id,
    selector: 'hero-form',
    templateUrl: './hero-form.component.html',
    styleUrls: ['./hero-form.component.css']
})
export class HeroFormComponent {

    powers = ['Really Smart', 'Super Flexible',
        'Super Hot', 'Weather Changer'];

    hero: Hero = { id: 18, name: 'Dr IQ', power: this.powers[0], alterEgo: 'Chuck Overstreet' };

    submitted = false;

    onSubmit() { this.submitted = true; }

    // TODO: Remove this when we're done
    get diagnostic() { return JSON.stringify(this.hero); }
}
