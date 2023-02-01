import './App.css';
import React, { useState } from 'react';
import GitHubProfile from './components/GitHubProfile';

const App: React.FC = () => {
  const [username, setUsername] = useState('');

  return (
      <div className="App">
        <input
            type="text"
            value={username}
            onChange={e => setUsername(e.target.value)}
            placeholder="Enter GitHub username"
        />
        {username && <GitHubProfile username={username} />}
      </div>
  );
}

export default App;

