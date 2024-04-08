<div align="center">
  <br />
  <h1>SGT (Total Management System)</h1>
  <p>
    Desktop system made in WPF/C# using the MVVM pattern in order to manage operational aspects of a company.
  </p>
  <p>
    Developed in .NET 6 and currently only available for desktop.
  </p>
</div>

## Important points

- This is a public repository in order to serve as a portfolio. Sensitive data (such as connections) has been hidden for security reasons, so commits do not reflect the actual development repository.
- This system was developed during a learning phase, so it may have issues regarding to best practices. Keep this in mind.
- The entire system (classes, interfaces, etc.) is written in pt-br.
- This system is an evolution made from scratch of the previous version that was developed in Windows Forms/VB.NET. Link to the repository: [SGT - Legacy](https://github.com/marcosmota5/SGT.Legacy).

## Functionalities

<ul>
  <li>System Access Control</li>
  <li>Encrypting User Passwords</li>
  <li>Sales proposals management
    <ul>
      <li>Insertion, editing and review of proposals with automatic code generation</li>
      <li>Visualization of proposals, being possible to export to different formats (PDF, Word, Excel etc.), also allowing sending by e-mail or replying to e-mails directly</li>
      <li>Import parts directly from quotations from suppliers</li>
      <li>Export parts directly to Excel</li>
      <li>Automatic calculation of the selling price of parts using the rates defined in advance which can be changed directly by the customer</li>
      <li>Automatic deadline update, checking items in stock and calculating deadlines according to availability</li>
      <li>Definition of shipping, with different logics available for apportionment of the total shipping price</li>
      <li>Setting discounts for all pieces or individually</li>
      <li>Management of services and displacements within the same proposal module</li>
      <li>Proposal search with several filter options available</li>
      <li>Among others</li>
    </ul>
  </li>
  <li>Service orders management
    <ul>
      <li>Entering and Editing Work Orders</li>
      <li>Export parts, events, inconsistencies directly to Excel</li>
      <li>Automatic search for machine information by equipment series or family</li>
      <li>Work order search with several filter options available</li>
      <li>Among others</li>
    </ul>
  </li>
  <li>Parameter management
    <ul>
      <li>Customers, contacts, manufacturers, types of equipment, models, families, years, series and many others</li>
    </ul>
  </li>
  <li>Others
    <ul>
      <li>Brand customization (customer logos)</li>
      <li>Save and apply filters</li>
      <li>Reorder tabs even being able to move them out creating an external window</li>
      <li>Integration between modules eg copying proposals to work orders and vice versa</li>
      <li>Creation and monitoring of tickets in order to facilitate communication between client and developer.</li>
    </ul>
  </li>
</ul>
  
## Contact

- [Email](mailto:marcosmota5@hotmail.com)

## License

This system is the property of Marcos Mota, and its distribution or copying is prohibited unless previously authorized by the owner.

The reason this repository is public is to serve as a portfolio for the developer.

Copyright © Marcos Oliveira Mota

## Dev Tools

Thanks to the tools/nuget packages below for providing great resources for free!

- [MahApps.Metro](https://github.com/MahApps/MahApps.Metro)
- [Serilog](https://github.com/serilog/serilog)

A big thanks to [SyncFusion](https://www.syncfusion.com/) for providing a lot of wonderful resources for free to individual developers and small businesses.

## Contributors

- [Marcos Mota / Developer](https://www.linkedin.com/in/marcosmota5/)
