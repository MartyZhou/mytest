import React, { Component } from 'react';
import logo from './logo.svg';
import './App.css';

class App extends Component {
  render() {
    const numbers = [1,2,3,4,5,7];
    return (
      <div className="App">
        <div className="App-header">
          <img src={logo} className="App-logo" alt="logo" />
          <h2>Welcome to React</h2>
        </div>
        <p className="App-intro">
          To get started, edit <code>src/App.js</code> and save to reload.
        </p>
        <Welcome name="Marty"/>
        <Clock />
        <NumberList numbers={numbers}/>
      </div>
    );
  }
}

function Welcome(props){
  return (
    <h1>Hello, {props.name}!</h1>
  );
}

class Clock extends Component {
  constructor(props){
    super(props);
    this.state = {date: new Date()};
  }

  componentDidMount(){
    this.timerId = setInterval(
      () => this.tick(),
      1000
    );
  }

  tick(){
    this.setState({date: new Date()});
  }

  render(){
    return (
      <div>
        <h2>It is {this.state.date.toLocaleTimeString()}.</h2>
      </div>
    );
  }
}

function NumberList(props){
  const numbers = props.numbers;
  const listItems = numbers.map((n) =>
    <li key={n.toString()}>
      {n}
    </li>
  );

  return (
    <ul>{listItems}</ul>
  );
}

export default App;
