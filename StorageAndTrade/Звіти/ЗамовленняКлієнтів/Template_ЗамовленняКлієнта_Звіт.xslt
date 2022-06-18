<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:output method="html" indent="yes" doctype-system="html" />

	<xsl:template name="Head">
		<meta charset="utf-8" />
		<meta name="viewport" content="width=device-width, initial-scale=1" />
		<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@4.6.1/dist/css/bootstrap.min.css" />
		<script src="https://cdn.jsdelivr.net/npm/jquery@3.6.0/dist/jquery.slim.min.js"></script>
		<script src="https://cdn.jsdelivr.net/npm/popper.js@1.16.1/dist/umd/popper.min.js"></script>
		<script src="https://cdn.jsdelivr.net/npm/bootstrap@4.6.1/dist/js/bootstrap.bundle.min.js"></script>
	</xsl:template>

    <xsl:template match="/root">

		<html>
			<head>
				<title>Table</title>
				<xsl:call-template name="Head" />
			</head>
			<body>

				<div class="container">
					<h2>Звіт</h2>
					<p>Замовлення клієнтів</p>

					<table class="table table-bordered table-sm">
						<thead>
							<tr>
								<th>Номенклатура</th>
								<th>Характеристика номенклатури</th>
								<th>Склад</th>
								<th>Замовлено</th>
								<th>Сума</th>
							</tr>
						</thead>

						<xsl:for-each select="row">
							<tbody>
								<tr>
									<td>
										<xsl:value-of select="Номенклатура_Назва"/>
									</td>
									<td>
										<xsl:value-of select="ХарактеристикаНоменклатури_Назва"/>
									</td>
									<td>
										<xsl:value-of select="Склад_Назва"/>
									</td>
									<td align="right">
										<xsl:value-of select="Замовлено"/>
									</td>
									<td align="right">
										<xsl:value-of select="Сума"/>
									</td>
								</tr>
							</tbody>
						</xsl:for-each>

					</table>

					<br/>
					<br/>
					<br/>
				    <br/>
				
				</div>

			</body>
		</html>
				
    </xsl:template>
	
</xsl:stylesheet>
