import { Injectable } from '@angular/core';

import { HEROES } from './mock-heroes';

@Injectable()
export class HeroService {
    getHeroes() {
        //return HEROES;
        return Promise.resolve(HEROES);
    }

    getHero(id) {
        //return this.getHeroes().find(h => h.id === id);
        return this.getHeroes()
            .then(heroes => heroes.find(hero => hero.id === id));
    }

}
