import React, { useState, useEffect } from 'react';
import axios from 'axios';

interface GitHubUser {
    Login: string;
    Id: number;
    AvatarUrl: string;
    Url: string;
    HtmlUrl: string;
    public_repos: number;
    bio: string;
    name: string;
    location: string;
    created_at: Date;
    IsFromCache: boolean;
}

interface Props {
    username: string;
}

const GitHubProfile: React.FC<Props> = ({ username }) => {
    const [user, setUser] = useState<GitHubUser | null>(null);
    const [error, setError] = useState<string | null>(null);
    const [cached, setCached] = useState<boolean>(false);

    useEffect(() => {
        const fetchData = async () => {
            try {
                // Check if the user data is in the cache
                const cacheResponse = await axios.get(`https://localhost:7215/api/github/${username}/cached`);
                if (cacheResponse.data) {
                    setUser(cacheResponse.data);
                    setCached(true);
                    return;
                }

                // If the user data is not in the cache, make a request to the GitHub API
                const response = await axios.get(`https://localhost:7215/api/github/${username}`);
                setUser(response.data);
                setCached(false);
            } catch (error) {
                setError((error as any).message);
            }
        };
        fetchData();
    }, [username]);

    if (error) {
        return <div>{error}</div>;
    }

    if (!user) {
        return <div>Loading...</div>;
    }

    return (
        <div>
            {cached && <p>Data from cache</p>}
            <img src={user.AvatarUrl} alt={`${user.Login}'s avatar`} />
            <p>Username: {user.Login}</p>
            <p>Name: {user.name}</p>
            <p>Bio: {user.bio}</p>
            <p>Location: {user.location}</p>
            <p>Number of public repositories: {user.public_repos}</p>
            <p>Profile URL: <a href={user.HtmlUrl}>{user.HtmlUrl}</a></p>
            <p>Is from cache: {user.IsFromCache ? 'Yes' : 'No'}</p>
        </div>
    );
};

export default GitHubProfile;

