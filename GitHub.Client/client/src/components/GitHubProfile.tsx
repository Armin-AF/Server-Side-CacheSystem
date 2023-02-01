import React, { useState } from 'react';
import axios from 'axios';
import './GitHubProfile.css';

interface GitHubUser {
    Login: string;
    Id: number;
    avatar_url: string;
    Url: string;
    html_url: string;
    public_repos: number;
    bio: string;
    name: string;
    location: string;
    created_at: Date;
    isFromCache: boolean;
}

const GitHubProfile: React.FC = () => {
    const [username, setUsername] = useState('');
    const [user, setUser] = useState<GitHubUser | null>(null);
    const [error, setError] = useState<string | null>(null);
    const [loading, setLoading] = useState(false);

    const fetchData = async (username: string) => {
        setLoading(true);
        try {
            const response = await axios.get(`https://localhost:7215/api/github/${username}`);
            setUser(response.data);
        } catch (error) {
            setError((error as any).message);
        }
        setLoading(false);
    };

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        fetchData(username);
    };

    return (
        <div className="github-profile">
            <form onSubmit={handleSubmit}>
                <input
                    type="text"
                    value={username}
                    onChange={e => setUsername(e.target.value)}
                    placeholder="Enter GitHub username"
                />
                <button type="submit">Search</button>
            </form>
            {loading && <div className="loading">Loading...</div>}
            {error && <div className="error">{error}</div>}
            {user && (
                <div className="user-info">
                    <img src={user.avatar_url} alt={`${user.Login}'s avatar`} />
                    <p className="username">Username: {user.Login}</p>
                    <p className="name">Name: {user.name}</p>
                    <p className="bio">Bio: {user.bio}</p>
                    <p className="location">Location: {user.location}</p>
                    <p className="public-repos">Number of public repositories: {user.public_repos}</p>
                    <p className="profile-url">Profile URL: <a href={user.html_url}>{user.html_url}</a></p>
                    <p className="cache">Is from cache: {user.isFromCache ? 'Yes' : 'No'}</p>
                </div>
            )}
        </div>
    );
};

export default GitHubProfile;



