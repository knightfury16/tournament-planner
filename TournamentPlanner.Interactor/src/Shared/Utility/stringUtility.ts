export function mapStringToEnum<T>(enumObj: T, value: string): T[keyof T] {
    return (enumObj as any)[value];
}