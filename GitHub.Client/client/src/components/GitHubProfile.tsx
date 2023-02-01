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

    const handleClear = () => {
        setUsername('');
        setUser(null);
        setError(null);
        setLoading(false);
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

            {loading && <div>Loading...</div>}
            {error && <div>{error}</div>}
            {user && (
                <>
                    <img src={user.avatar_url} alt={`${user.Login}'s avatar`} />
                    <p>Username: {user.Login}</p>
                    <p>Name: {user.name}</p>
                    <p>Bio: {user.bio}</p>
                    <p>Location: {user.location}</p>
                    <p>Number of public repositories: {user.public_repos}</p>
                    <p>Profile URL: <a href={user.html_url}>{user.html_url}</a></p>
                    <p>Is from cache: {user.isFromCache ? 'Yes' : 'No'}</p>
                </>
            )}
            <button className={"clear-button"} type="button" onClick={handleClear}>Clear</button>
        </div>
    );
};

export default GitHubProfile;




