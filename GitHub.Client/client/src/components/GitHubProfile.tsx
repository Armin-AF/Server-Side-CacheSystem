import React, { useState } from 'react';
import axios from 'axios';
import './GitHubProfile.css';

interface GitHubUser {
    login: string;
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
    const [timeTaken, setTimeTaken] = useState<number | null>(null);

    const fetchData = async (username: string) => {
        setLoading(true);
        const startTime = Date.now();
        try {
            const response = await axios.get(`https://localhost:7215/api/github/${username}`);
            setUser(response.data);
        } catch (error) {
            setError((error as any).message);
        }
        const endTime = Date.now();
        setTimeTaken(endTime - startTime);
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
            <img src={"https://github.githubassets.com/images/modules/logos_page/GitHub-Mark.png"} alt={"GitHub logo"} />
            <h1>GitHub Profile</h1>
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
                    <img src={user.avatar_url} alt={`${user.login}'s avatar`} />
                    <p>Username: {user.login}</p>
                    <p>Name: {user.name}</p>
                    <p>Bio: {user.bio}</p>
                    <p>Location: {user.location}</p>
                    <p>Number of public repositories: {user.public_repos}</p>
                    <p>Profile URL: <a href={user.html_url}>{user.html_url}</a></p>
                    <p>Is from cache: {user.isFromCache ? 'Yes' : 'No'}</p>
                    <p>Time taken: {timeTaken}ms</p>
                </>
            )}
            <button className={"clear-button"} type="button" onClick={handleClear}>Clear</button>
        </div>
    );
};

export default GitHubProfile;




