import 'reflect-metadata'; // es7.decorators requires reflect-metadata
import 'zone.js' // Angular requires Zone.js prolyfill.
import 'rxjs' // ngModel requires this.

import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { AppModule } from './app/app.module';

platformBrowserDynamic().bootstrapModule(AppModule);