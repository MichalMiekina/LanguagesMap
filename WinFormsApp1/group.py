import json
import pandas as pd
from sklearn.cluster import DBSCAN
import numpy as np
import matplotlib.pyplot as plt
import seaborn as sns
from unidecode import unidecode
from transliterate import translit
from thefuzz import fuzz

# Load the JSON data from file
with open('translations.json', 'r', encoding='utf-8') as f:
    data = json.load(f)

# Function to transliterate and normalize text
def normalize_text(text):
    try:
        # Attempt to transliterate using the transliterate library
        transliterated = translit(text, reversed=True)
    except:
        # If transliteration fails, fallback to unidecode
        transliterated = unidecode(text)
    # Normalize text by converting to lowercase and stripping accents
    normalized = unidecode(transliterated).lower()
    return normalized

# Normalize the TranslationResult entries and add a new field for it
for entry in data:
    entry['TranslationResultLatin'] = normalize_text(entry['TranslationResult'])

# Convert JSON data to a pandas DataFrame
df = pd.DataFrame(data)

# Compute the fuzzy similarity matrix
similarity_matrix = np.zeros((len(df), len(df)))
for i in range(len(df)):
    for j in range(len(df)):
        similarity_matrix[i, j] = fuzz.ratio(df.loc[i, 'TranslationResultLatin'], df.loc[j, 'TranslationResultLatin'])

# Convert the similarity matrix to a distance matrix
distance_matrix = 100 - similarity_matrix

# Perform DBSCAN clustering using the distance matrix
dbscan = DBSCAN(eps=30, min_samples=1, metric='precomputed')
df['Cluster'] = dbscan.fit_predict(distance_matrix)

# Handle outliers (cluster -1) by assigning them to a new cluster number
max_cluster = df['Cluster'].max()
df['Cluster'] = df['Cluster'].apply(lambda x: max_cluster + 1 if x == -1 else x)

# Convert the DataFrame back to a list of dictionaries
updated_data = df.to_dict(orient='records')

# Save the updated data to a new JSON file
with open('updated_translations.json', 'w', encoding='utf-8') as f:
    json.dump(updated_data, f, ensure_ascii=False, indent=4)

# Print the clustered data
print(df)

# Visualize the clusters
plt.figure(figsize=(10, 6))
sns.countplot(x='Cluster', data=df, palette='viridis')
plt.title('Cluster Distribution of Translation Results')
plt.xlabel('Cluster')
plt.ylabel('Number of Countries')
plt.show()

# Show the cluster groups
for cluster_num in sorted(df['Cluster'].unique()):
    cluster_group = df[df['Cluster'] == cluster_num]
    print(f"\nCluster {cluster_num}:")
    print(cluster_group[['CountryName', 'TranslationResult', 'TranslationResultLatin']])
