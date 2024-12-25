import { TournamentDto } from "../../app/tp-model/TpModel";

export function transformIsoTimeStringToDate(value: string | null): string | null {
    if (value === null) return null;
    const date = new Date(value);
    return date.toISOString().split('T')[0];
}


export function getDateStringInFormat(date: Date): string {
    const year = date.getFullYear();
    const month = date.toLocaleString('default', { month: 'short' });
    const day = date.getDate().toString().padStart(2, '0');
    return `${day} ${month} ${year}`;
}


export function transformTournamentIsoDate(value: TournamentDto | null): TournamentDto | null {
    if (!value) return null;
    const startDate = value.startDate ? getDateStringInFormat(new Date(value.startDate)) : null;
    const endDate = value.endDate ? getDateStringInFormat(new Date(value.endDate)) : null;
    const registrationLastDate = value.registrationLastDate ? getDateStringInFormat(new Date(value.registrationLastDate)) : null;
    const createdAt = value.createdAt ? getDateStringInFormat(new Date(value.createdAt)) : null;
    const updateAt = value.updateAt ? getDateStringInFormat(new Date(value.updateAt)) : null;
    return {
        ...value,
        startDate: startDate,
        endDate: endDate,
        registrationLastDate,
        createdAt,
        updateAt
    };
}



