import React, { Component } from 'react';

class HeroForm extends Component {
    constructor(props){
        super(props);
        this.state = {
            id: 1,
            name: 'Dr IQ',
            alterEgo: 'Strong',
            power: 'Super Hot'
        };

        this.powers = ['Really Smart', 'Super Flexible', 'Super Hot', 'Weather Changer'];

        this.handleNameChange = this.handleNameChange.bind(this);
        this.handleAlterEgoChange = this.handleAlterEgoChange.bind(this);
        this.handlePowerChange = this.handlePowerChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }

    handleNameChange(event){
        this.setState({name: event.target.value});
    }

    handleAlterEgoChange(event){
        this.setState({alterEgo: event.target.value});
    }

    handlePowerChange(event){
        this.setState({power: event.target.value});
    }

    handleSubmit(event){
        alert('A name was submitted: ' + JSON.stringify(this.state));
        event.preventDefault();
    }

    render(){
        const powerOptions = this.powers.map((p) => 
        <option key={p}>{p}</option>
        );

        return (
            <div className="container">
    <h1>Hero Form</h1>
    <form onSubmit={this.handleSubmit}>
        <div className="form-group">
            <label htmlFor="name">Name</label>
            <input type="text" className="form-control" id="name" required name="name" value={this.state.name} onChange={this.handleNameChange}/>
        </div>

        <div className="form-group">
            <label htmlFor="alterEgo">Alter Ego</label>
            <input type="text" className="form-control" id="alterEgo" name="alterEgo" value={this.state.alterEgo} onChange={this.handleAlterEgoChange}/>
        </div>

        <div className="form-group">
            <label htmlFor="power">Hero Power</label>
            <select className="form-control" id="power" required name="power" value={this.state.power} onChange={this.handlePowerChange}>
                {powerOptions}
            </select>
        </div>

        <button type="submit" className="btn btn-success">Submit</button>

    </form>
</div>
        );
    }
} 

export default HeroForm;