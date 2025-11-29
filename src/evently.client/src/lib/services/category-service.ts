import { Category } from "~/domains/entities";
import axios from "axios";

export async function getCategories(): Promise<Category[]> {
	const response = await axios.get<Category[]>("/api/v1/Categories");
	return response.data;
}
