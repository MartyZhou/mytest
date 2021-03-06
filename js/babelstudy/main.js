import 'reflect-metadata'; // es7.decorators requires reflect-metadata
import 'zone.js' // Angular requires Zone.js prolyfill.
import 'rxjs' // ngModel requires this.

//import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { AppModule } from './app/app.module';

//enableProdMode();

platformBrowserDynamic().bootstrapModule(AppModule);