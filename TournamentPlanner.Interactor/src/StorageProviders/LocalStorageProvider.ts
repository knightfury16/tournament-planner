import { IStorageProvider } from "./IStorageProvider";

export class LocalStorageProvider implements IStorageProvider {
    async get(key: string): Promise<string | null> {
        if(key == undefined || key == null) return null;
        return localStorage.getItem(key);
    }
    async set(key: string, value: string): Promise<void> {
        localStorage.setItem(key, value)
    }
    async remove(key: string): Promise<void> {
        localStorage.removeItem(key)
    }
    async clear(): Promise<void> {
        localStorage.clear();
    }
}