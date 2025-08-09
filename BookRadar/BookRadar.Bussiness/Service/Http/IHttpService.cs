namespace BookRadar.Bussiness.Service.Http
{
    /// <summary>
    /// Contrato para un servicio HTTP que permite realizar operaciones comunes
    /// como GET, POST, PUT y DELETE.
    /// </summary>
    public interface IHttpService
    {
        /// <summary>
        /// Realiza una solicitud HTTP GET a la URL especificada y deserializa
        /// la respuesta en un objeto del tipo especificado.
        /// </summary>
        /// <typeparam name="T">El tipo del objeto en el que se deserializará la respuesta.</typeparam>
        /// <param name="url">La URL a la que se enviará la solicitud GET.</param>
        /// <param name="token">El JWT a la que se enviará la solicitud GET.</param>
        /// <returns>
        /// Una tarea que representa la operación asincrónica.
        /// El resultado contiene el objeto deserializado de tipo <typeparamref name="T"/>
        /// o <c>null</c> si la respuesta no es válida o está vacía.
        /// </returns>
        Task<T> GetAsync<T>(string url, string token = null);

        Task<byte[]> GetFileAsync(string url, string token = null);

        /// <summary>
        /// Realiza una solicitud HTTP POST a la URL especificada con los datos proporcionados
        /// y deserializa la respuesta en un objeto del tipo especificado.
        /// </summary>
        /// <typeparam name="TRequest">El tipo de los datos que se enviarán en el cuerpo de la solicitud.</typeparam>
        /// <typeparam name="TResponse">El tipo del objeto en el que se deserializará la respuesta.</typeparam>
        /// <param name="url">La URL a la que se enviará la solicitud POST.</param>
        /// <param name="data">Los datos que se incluirán en el cuerpo de la solicitud.</param>
        /// <param name="isForm">Indica si los datos deben ser enviados como un formulario (<c>true</c>) o como JSON (<c>false</c>).</param>
        /// <param name="token">El JWT a la que se enviará la solicitud POST.</param>
        /// <returns>
        /// Una tarea que representa la operación asincrónica.
        /// El resultado contiene el objeto deserializado de tipo <typeparamref name="TResponse"/>
        /// o <c>null</c> si la respuesta no es válida o está vacía.
        /// </returns>
        Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest data, bool isForm, string token = null);

        /// <summary>
        /// Realiza una solicitud HTTP PUT a la URL especificada con los datos proporcionados
        /// y deserializa la respuesta en un objeto del tipo especificado.
        /// </summary>
        /// <typeparam name="TRequest">El tipo de los datos que se enviarán en el cuerpo de la solicitud.</typeparam>
        /// <typeparam name="TResponse">El tipo del objeto en el que se deserializará la respuesta.</typeparam>
        /// <param name="url">La URL a la que se enviará la solicitud PUT.</param>
        /// <param name="data">Los datos que se incluirán en el cuerpo de la solicitud.</param>
        /// <param name="isForm">Indica si los datos deben ser enviados como un formulario (<c>true</c>) o como JSON (<c>false</c>).</param>
        /// <param name="token">El JWT a la que se enviará la solicitud PUT.</param>
        /// <returns>
        /// Una tarea que representa la operación asincrónica.
        /// El resultado contiene el objeto deserializado de tipo <typeparamref name="TResponse"/>
        /// o <c>null</c> si la respuesta no es válida o está vacía.
        /// </returns>
        Task<TResponse> PutAsync<TRequest, TResponse>(string url, TRequest data, bool isForm, string token = null);

        /// <summary>
        /// Realiza una solicitud HTTP DELETE a la URL especificada y deserializa la respuesta en un objeto del tipo especificado.
        /// </summary>
        /// <typeparam name="TResponse">El tipo del objeto en el que se deserializará la respuesta.</typeparam>
        /// <param name="url">La URL a la que se enviará la solicitud DELETE.</param>
        /// <param name="token">El JWT a la que se enviará la solicitud GET.</param>
        /// <returns>
        /// Una tarea que representa la operación asincrónica.
        /// El resultado contiene el objeto deserializado de tipo <typeparamref name="TResponse"/>
        /// o <c>false</c> si la operación falló.
        /// </returns>
        Task<TResponse> DeleteAsync<TResponse>(string url, string token = null);

        Task<(byte[] contenido, string nombreArchivo)> PostForFileAsync<TRequest>(string url, TRequest data, string token = null);
    }
}