import React, { Component } from 'react';

class Dashboard extends Component {
    constructor(){
        super();

        this.heroes = [
      {id: 11, name: 'Mr. Nice'},
      {id: 12, name: 'Narco'},
      {id: 13, name: 'Bombasto'},
      {id: 14, name: 'Celeritas'}
        ];
    }

    render(){
        const itemList = this.heroes.map((h)=>
            <a key={h.id} className="col-1-4">
                <div className="module hero">
                <h4>{h.name}</h4>
                </div>
            </a>
        );
        return (
            <div>
            <h3>Top Heroes</h3>
            <div className="grid grid-pad">
            {itemList}
            </div>
            </div>
        );
    }
}

export default Dashboard; 