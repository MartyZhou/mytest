import React from 'react';
import ReactDOM from 'react-dom';
import App from './App';
import './index.css';

const numbers = [1, 2, 3, 4, 5];
const doubled = numbers.map((number) => number * 2);
console.log(doubled);

ReactDOM.render(
  <App />,
  document.getElementById('root')
);
