export function mapStringToEnum<T>(enumObj: T, value: string): T[keyof T] {
    return (enumObj as any)[value];
}

export function trimAllSpace(input: string): string {
  return input.replace(/\s+/g, '');
}
