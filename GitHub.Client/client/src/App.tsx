import './App.css';
import React, { useState } from 'react';
import GitHubProfile from './components/GitHubProfile';

const App: React.FC = () => {


  return (
      <div className="App">
            <GitHubProfile/>
      </div>
  );
}

export default App;

