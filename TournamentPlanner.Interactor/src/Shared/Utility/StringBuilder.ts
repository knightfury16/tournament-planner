//I come from dotnet land
export class StringBuilder {
    private parts: string[] = [];

    public append(text: string): this {
        this.parts.push(text);
        return this;
    }

    public toString(): string {
        return this.parts.join('');
    }
}