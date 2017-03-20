import { InMemoryDbService } from 'angular-in-memory-web-api';
export class InMemoryDataService implements InMemoryDbService {
  createDb() {
    let heroes = [
      {id: 11, name: 'Mr. Nice', power: 'Really Smart'},
      {id: 12, name: 'Narco', power: 'Super Flexible'},
      {id: 13, name: 'Bombasto', power: 'Super Hot'},
      {id: 14, name: 'Celeritas', power: 'Weather Changer'},
      {id: 15, name: 'Magneta', power: 'Super Hot'},
      {id: 16, name: 'RubberMan', power: 'Super Flexible'},
      {id: 17, name: 'Dynama', power: 'Really Smart'},
      {id: 18, name: 'Dr IQ', power: 'Super Flexible'},
      {id: 19, name: 'Magma', power: 'Weather Changer'},
      {id: 20, name: 'Tornado', power: 'Super Hot'}
    ];
    return {heroes};
  }
}
